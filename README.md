# 🍝 Love&Cooking – Sito Ricette con Gestionale e Tracciamento Utente

Progetto completo per la gestione e pubblicazione di ricette con architettura a tre moduli:

- **Frontend Angular** per la visualizzazione pubblica e gestionale
- **Backend ASP.NET Core** per la gestione di categorie, ricette, immagini, utenti
- **Microservizio di Tracking** per il tracciamento avanzato degli eventi utente

---

## 📦 Struttura del progetto

```
Root/
├── Backend/ → ASP.NET Core API
├── Front-end/ → App Angular (utente e admin)
└── Microservizio/ → Tracking Service (RabbitMQ, ETL)
```

---

## 💡 Funzionalità principali

### 🎨 Frontend (Angular)

- Navigazione ricette, categorie, blog
- Pannello admin con form dinamici
- Caricamento immagini con metadati SEO
- Responsive e ottimizzato per Google PageSpeed

### 🛠 Backend (.NET 8 API)

- CRUD completo di categorie, ricette, ingredienti
- Upload immagini con metadata (alt, titolo, descrizione)
- Entity Framework + SQL Server
- API REST ottimizzate, valide per futuro e-commerce

### 📊 Microservizio Tracking

- Tracciamento eventi utente via RabbitMQ
- Archiviazione eventi e sessioni per analisi
- ETL per ricostruzione funnel e comportamento utente

---

## 🚀 Avvio locale

### Backend

```bash
cd Backend/CucinaMammaAPI
dotnet run
```

### Frontend

```bash
cd Front-end/sito-cucina-mamma
npm install
ng serve
```

### Microservizio

```bash
cd Microservizio/TrackingService
dotnet run
```

### ⚙️ Tecnologie usate

- Frontend: Angular 19, RxJS, SCSS
- Backend: ASP.NET Core 8, Entity Framework Core
- Microservizio: Worker .NET, RabbitMQ
- Database: SQL Server
- DevOps: GitHub Actions, CI/CD

### 📚 Documentazione

- docs/ARCHITETTURA.md → Struttura tecnica e flusso dati
- .github/workflows/ci.yml → Build automatica

### 📈 Sviluppi futuri

- Area blog e commenti
- E-commerce digitale per ricettari
- Integrazione Health & Cooking
- Dashboard statistica e marketing
