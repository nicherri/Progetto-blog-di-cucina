using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using TrackingService.Data;
using TrackingService.Models;
using SharedModels.Models;
using System.Globalization;

namespace TrackingService.Services;

public class EtlHostedService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(5);

    public EtlHostedService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await RunEtlOnce(stoppingToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EtlHostedService] ERRORE: {ex.Message}");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }

    private async Task RunEtlOnce(CancellationToken token)
    {
        using var scope = _scopeFactory.CreateScope();
        var opDb = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var analyticsDb = scope.ServiceProvider.GetRequiredService<AnalyticsDbContext>();

        var events = await opDb.Events
            .Include(e => e.Session)
            .Where(e => !e.IsImported)
            .OrderBy(e => e.TimestampUtc)
            .Take(1000)
            .ToListAsync(token);

        if (!events.Any())
        {
            Console.WriteLine("[EtlHostedService] Nessun evento da importare");
            return;
        }

        Console.WriteLine($"[EtlHostedService] Trovati {events.Count} eventi da importare");

        foreach (var ev in events)
        {
            var userSK = await GetOrCreateUserSKAsync(analyticsDb, ev.UserId);
            var sessionSK = await GetOrCreateSessionSKAsync(analyticsDb, ev.Session);
            var dateSK = await GetOrCreateDateSKAsync(analyticsDb, ev.TimestampUtc);
            var timeSK = await GetOrCreateTimeSKAsync(analyticsDb, ev.TimestampUtc);
            var eventSK = await GetOrCreateEventSKAsync(analyticsDb, ev);
            var deviceSK = await GetOrCreateDeviceSKAsync(analyticsDb, ev);
            var locationSK = await GetOrCreateLocationSKAsync(analyticsDb, ev);

            var fact = new FactEvent
            {
                UserSK = userSK,
                SessionSK = sessionSK,
                DateSK = dateSK,
                TimeSK = timeSK,
                EventSK = eventSK,
                DeviceSK = deviceSK,
                LocationSK = locationSK,

                EventName = ev.EventName,
                EventCategory = ev.EventCategory,
                EventLabel = ev.EventLabel,
                EventValue = ev.EventValue,
                MouseX = ev.MouseX,
                MouseY = ev.MouseY,
                ScrollX = ev.ScrollX,
                ScrollY = ev.ScrollY,
                ViewportWidth = ev.ViewportWidth,
                ViewportHeight = ev.ViewportHeight,
                ElementLeft = ev.ElementLeft,
                ElementTop = ev.ElementTop,
                ElementWidth = ev.ElementWidth,
                ElementHeight = ev.ElementHeight,
                FunnelStep = ev.FunnelStep,
                FunnelData = ev.FunnelData,
                ReplayChunkData = ev.ReplayChunkData,
                ReplayChunkType = ev.ReplayChunkType,
                ScrollDepthPercentage = ev.ScrollDepthPercentage,
                TimeSpentSeconds = (decimal?)ev.TimeSpentSeconds,
                TrackingVersion = 1,
                TimestampUtc = ev.TimestampUtc,
                DataCaricamento = DateTime.UtcNow
            };

            analyticsDb.FactEvents.Add(fact);
            ev.IsImported = true;
        }

        await analyticsDb.SaveChangesAsync(token);
        await opDb.SaveChangesAsync(token);

        Console.WriteLine($"[EtlHostedService] Importati {events.Count} eventi.");
    }


    private async Task<int> GetOrCreateUserSKAsync(AnalyticsDbContext db, string? userId)
    {
        var effectiveUserId = string.IsNullOrEmpty(userId) ? "ANONYMOUS" : userId;

        var user = await db.DimUsers.FirstOrDefaultAsync(u => u.UserID == effectiveUserId);
        if (user == null)
        {
            user = new DimUser
            {
                UserID = effectiveUserId,
                IsCurrent = true,
                Versione = 1,
                DataInizioValidita = DateTime.UtcNow,
                DataCaricamento = DateTime.UtcNow
            };
            db.DimUsers.Add(user);
            await db.SaveChangesAsync();
        }

        return user.UserSK;
    }

    private async Task<int> GetOrCreateDeviceSKAsync(AnalyticsDbContext db, EventTracking ev)
    {
        var existing = await db.DimDevices
            .FirstOrDefaultAsync(d => d.DeviceID == ev.SessionId && d.DeviceType == ev.Session.DeviceType);

        if (existing != null)
            return existing.DeviceSK;

        var newDevice = new DimDevice
        {
            DeviceID = ev.SessionId,
            DeviceType = ev.Session.DeviceType,
            OS = ev.Session.OperatingSystem,
            Browser = ev.Session.BrowserInfo,
            BrowserVersion = null,
            DataCaricamento = DateTime.UtcNow
        };

        db.DimDevices.Add(newDevice);
        await db.SaveChangesAsync();

        return newDevice.DeviceSK;
    }
    private async Task<int> GetOrCreateLocationSKAsync(AnalyticsDbContext db, EventTracking ev)
    {
        var existing = await db.DimLocations
            .FirstOrDefaultAsync(l => l.Country == ev.Session.Country && l.City == ev.Session.City);

        if (existing != null)
            return existing.LocationSK;

        var newLocation = new DimLocation
        {
            Country = ev.Session.Country,
            City = ev.Session.City,
            Region = ev.Session.Region,
            CountryISOCode = null,
            PostalCode = null,
            Latitude = null,
            Longitude = null,
            DataCaricamento = DateTime.UtcNow
        };

        db.DimLocations.Add(newLocation);
        await db.SaveChangesAsync();

        return newLocation.LocationSK;
    }

    private async Task<int> GetOrCreateDateSKAsync(AnalyticsDbContext db, DateTime dateTime)
    {
        var date = dateTime.Date;
        var existing = await db.DimDates.FirstOrDefaultAsync(d => d.FullDate == date);

        if (existing != null)
            return existing.DateSK;

        var newDate = new DimDate
        {
            FullDate = date,
            Year = date.Year,
            Quarter = (date.Month - 1) / 3 + 1,
            Month = date.Month,
            MonthName = date.ToString("MMMM"),
            Day = date.Day,
            DayOfWeek = (int)date.DayOfWeek,
            DayNameOfWeek = date.DayOfWeek.ToString(),
            WeekOfYear = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday),
            IsHoliday = false,
            IsWeekend = date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday,
            FiscalYear = null,
            FiscalQuarter = null,
            DataCaricamento = DateTime.UtcNow
        };

        db.DimDates.Add(newDate);
        await db.SaveChangesAsync();

        return newDate.DateSK;
    }

    private async Task<int> GetOrCreateTimeSKAsync(AnalyticsDbContext db, DateTime dateTime)
    {
        var existing = await db.DimTimes
            .FirstOrDefaultAsync(t => t.Hour == dateTime.Hour && t.Minute == dateTime.Minute);

        if (existing != null)
            return existing.TimeSK;

        var newTime = new DimTime
        {
            Hour = (byte)dateTime.Hour,
            Minute = (byte)dateTime.Minute,
            AM_PM = dateTime.Hour < 12 ? "AM" : "PM",
            TimeSlot = $"{dateTime.Hour:00}:00-{dateTime.Hour:00}:59",
            QuarterHourSlot = $"{dateTime.Hour:00}:{(dateTime.Minute / 15) * 15:00}",
            DataCaricamento = DateTime.UtcNow
        };

        db.DimTimes.Add(newTime);
        await db.SaveChangesAsync();

        return newTime.TimeSK;
    }

    private async Task<int> GetOrCreateEventSKAsync(AnalyticsDbContext db, EventTracking ev)
    {
        var existing = await db.DimEvents
            .FirstOrDefaultAsync(e => e.EventID == ev.EventName);

        if (existing != null)
            return existing.EventSK;

        var newEvent = new DimEvent
        {
            EventID = ev.EventName,
            EventName = ev.EventName,
            EventCategory = ev.EventCategory,
            EventLabel = ev.EventLabel,
            DataInizioValidita = DateTime.UtcNow,
            IsCurrent = true,
            Versione = 1,
            DataCaricamento = DateTime.UtcNow
        };

        db.DimEvents.Add(newEvent);
        await db.SaveChangesAsync();

        return newEvent.EventSK;
    }

    private async Task<int> GetOrCreateSessionSKAsync(AnalyticsDbContext db, SessionTracking session)
    {
        var existing = await db.DimSessions
            .FirstOrDefaultAsync(s => s.SessionID == session.Id);

        if (existing != null)
            return existing.SessionSK;

        var newSession = new DimSession
        {
            SessionID = session.Id,
            SessionStart = session.StartTimeUtc,
            SessionEnd = session.EndTimeUtc,
            SessionStatus = session.EndTimeUtc.HasValue ? "Closed" : "Open",
            TipoSessione = null,
            DataInizioValidita = DateTime.UtcNow,
            IsCurrent = true,
            Versione = 1,
            BrowserInfo = session.BrowserInfo,
            IpAddress = session.IpAddress,
            OptOut = session.OptOut,
            PageViews = session.PageViews,
            Locale = session.Locale,
            OperatingSystem = session.OperatingSystem,
            Country = session.Country,
            Region = session.Region,
            City = session.City,
            UserId = session.UserId,
            DeviceType = session.DeviceType,
            DataCaricamento = DateTime.UtcNow
        };

        db.DimSessions.Add(newSession);
        await db.SaveChangesAsync();

        return newSession.SessionSK;
    }




}
