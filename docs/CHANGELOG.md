# 📜 Changelog – Love&Cooking

Tutti i cambiamenti importanti a questo progetto saranno documentati in questo file.

Il formato segue [Keep a Changelog](https://keepachangelog.com/it/1.0.0/)  
e aderisce alla versione [SemVer](https://semver.org/lang/it/).

---

## [Unreleased]

### ✨ Aggiunte

- Integrazione `LICENSE` MIT
- Aggiunta `README.md` con struttura completa del progetto
- Aggiunta cartella `docs/` con `ARCHITETTURA.md` e `Features.md`
- Configurazione `GitHub Actions` per build frontend e backend
- Microservizio tracking con eventi via RabbitMQ
- Upload immagini con metadati SEO + ordinamento via drag & drop
- Validazione form Angular con evidenziazione errori

---

## [2025-05-05] – Versione iniziale

### 🚀 Primo commit completo

- Struttura monorepo con:
  - Frontend Angular 19
  - Backend ASP.NET Core 8
  - Microservizio tracking eventi (.NET Worker)
- CRUD completo per categorie, ricette, immagini e ingredienti
- Upload immagini con metadati
- Tracciamento eventi utente e sessione
- Documentazione iniziale (`README.md`, `ARCHITETTURA.md`, `Features.md`)
- Flusso CI/CD configurato (GitHub Actions)
