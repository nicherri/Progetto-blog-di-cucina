// /core/models/event-tracking.model.ts
export interface EventTracking {
  sessionId: string | null;
  userId?: string; // Se l'utente è loggato
  eventName: string; // "PageView", "RecipeClick", ...
  eventCategory?: string; // "Navigazione", "Ricetta"
  eventLabel?: string; // "HomepageBanner"
  eventValue?: number;
  pageUrl?: string;
  referrer?: string;
  timestampUtc?: string;
  timeSpentSeconds?: number;
  scrollDepthPercentage?: number;
  additionalData?: string; // JSON string con info extra
  optOut?: boolean; // Se vogliamo indicare un “opt-out” a livello di singolo evento

  mouseX?: number;
  mouseY?: number;
  scrollX?: number;
  scrollY?: number;
  viewportWidth?: number;
  viewportHeight?: number;
  elementLeft?: number;
  elementTop?: number;
  elementWidth?: number;
  elementHeight?: number;
  replayChunkData?: any;
  replayChunkType?: string;
  funnelData?: any;
}
