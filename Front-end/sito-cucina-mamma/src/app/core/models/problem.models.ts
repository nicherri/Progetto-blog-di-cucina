export interface ApiProblem {
  status: number;
  title: string;
  userMessage?: string;
  severity?: 'Info' | 'Warning' | 'Error' | 'Fatal';
  errors?: Record<string, string[]>;
  retryAfter?: number;
}

export const Severity2Type = {
  Info: 'success',
  Warning: 'warning',
  Error: 'error',
  Fatal: 'error',
} as const;
