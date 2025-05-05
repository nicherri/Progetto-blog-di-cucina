import 'zone.js/node';
import express from 'express';
import { join } from 'path';
import { readFileSync } from 'fs';
import bootstrap from './main.server';

import { renderApplication } from '@angular/platform-server';
import { config as serverConfig } from './app/app.config.server';

const DIST_FOLDER = join(process.cwd(), 'dist/sito-cucina-mamma/browser');

const app = express();

app.get('*', async (req, res) => {
  try {
    const html = await renderApplication(bootstrap, {
      document: readFileSync(join(DIST_FOLDER, 'index.html')).toString(),
      url: req.originalUrl,
    });

    res.status(200).send(html);
  } catch (err) {
    console.error('❌ Errore SSR:', err);
    res.status(500).send('Errore durante il rendering SSR');
  }
});
