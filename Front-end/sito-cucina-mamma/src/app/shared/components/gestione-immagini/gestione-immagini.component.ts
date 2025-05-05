import { Component, EventEmitter, Output, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ImmagineForm } from '../../../core/models/ImmagineForm';

import {
  CdkDropList,
  CdkDragDrop,
  CdkDragMove,
  CdkDragStart,
  moveItemInArray,
  DragDropModule,
} from '@angular/cdk/drag-drop';
import { ImmagineMetadatiModalComponent } from '../immagine-metadati-modal/immagine-metadati-modal.component';
import { IconCloseComponent } from '../../Icons/icon-close/icon-close.component';
import { IconDotsComponent } from '../../Icons/icon-dots/icon-dots.component';
import { IconPlusComponent } from '../../Icons/icon-plus/icon-plus.component';
import { IconWarningComponent } from '../../Icons/icon-warning/icon-warning.component';
import ColorThief from 'colorthief';
import {
  trigger,
  transition,
  query,
  style,
  animate,
} from '@angular/animations';

@Component({
  selector: 'app-gestione-immagini',
  standalone: true,
  imports: [
    CommonModule,
    DragDropModule,
    ImmagineMetadatiModalComponent,
    IconCloseComponent,
    IconDotsComponent,
    IconPlusComponent,
    IconWarningComponent,
  ],
  templateUrl: './gestione-immagini.component.html',
  styleUrls: ['./gestione-immagini.component.scss'],
  animations: [
    trigger('spostaCard', [
      transition('* => *', [
        query(
          ':enter, :leave',
          [
            style({ transform: 'translateX(0)' }),
            animate(
              '150ms ease-in-out',
              style({ transform: 'translateX(5px)' }),
            ),
          ],
          { optional: true },
        ),
      ]),
    ]),
  ],
})
export class GestioneImmaginiComponent {
  immagini: ImmagineForm[] = [];
  dropzoneAttiva = false;
  modaleAttivo = false;
  immagineSelezionata?: ImmagineForm;
  indiceSelezionato?: number;
  immagineZoom?: string;
  modaleZoomAttivo = false;
  tooltipVisibile = false;
  indiceTooltip: number | null = null;
  indiceInserimento: number | null = null;
  inDragInterno = false;
  counterDrag = 0;
  posizioneInserimentoPx: number = 0;
  posizioneInserimentoTopPx: number = 0;
  larghezzaInserimentoPx: number = 0;
  scrollInterval: any;

  @Output() immaginiAggiornate = new EventEmitter<ImmagineForm[]>();

  @HostListener('document:dragenter', ['$event'])
  onGlobalDragEnter(event: DragEvent): void {
    event.preventDefault();
    this.counterDrag++;
    this.dropzoneAttiva = true;
  }

  @HostListener('document:dragleave', ['$event'])
  onGlobalDragLeave(event: DragEvent): void {
    event.preventDefault();
    this.counterDrag--;
    if (this.counterDrag === 0) {
      this.dropzoneAttiva = false;
    }
  }

  @HostListener('document:dragover', ['$event'])
  onGlobalDragOver(event: DragEvent): void {
    event.preventDefault();
  }

  @HostListener('document:drop', ['$event'])
  onGlobalDrop(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation(); // Ferma la propagazione dell'evento
    this.counterDrag = 0;
    this.dropzoneAttiva = false;
    this.processaFileDrop(event);
  }

  // ======= GESTIONE DRAG & DROP SUL BOTTONE + =======

  onDragOver(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.dropzoneAttiva = true;
  }

  onDragLeave(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.dropzoneAttiva = false;
  }

  // ======= PROCESSA FILE =======

  processaFileDrop(event: DragEvent): void {
    const files = event.dataTransfer?.files;
    if (files && files.length > 0) {
      Array.from(files).forEach((file) => {
        const reader = new FileReader();
        reader.onload = async () => {
          const anteprima = reader.result as string;
          const nuovaImmagine: ImmagineForm = {
            alt: '',
            caption: '',
            isCover: false,
            nomeFileSeo: '',
            ordine: this.immagini.length,
            title: '',
            file,
            anteprima,
            url: '',
          };
          this.immagini.push(nuovaImmagine);
          this.immaginiAggiornate.emit(this.immagini);
        };
        reader.readAsDataURL(file);
      });
    }
  }
  // ======= FILE INPUT =======
  apriFileInput(): void {
    const fileInput = document.getElementById('fileInput') as HTMLInputElement;
    fileInput?.click();
  }

  onFileChange(event: Event): void {
    event.preventDefault();
    event.stopPropagation();
    this.dropzoneAttiva = false;

    const input = event.target as HTMLInputElement;
    const files = input.files || (event as DragEvent).dataTransfer?.files;

    if (files) {
      Array.from(files).forEach((file) => {
        const reader = new FileReader();
        reader.onload = async () => {
          const anteprima = reader.result as string;
          const imgElement = new Image();
          imgElement.src = anteprima;
          imgElement.crossOrigin = 'Anonymous';

          imgElement.onload = async () => {
            const colorThief = new ColorThief();
            let dominante = [255, 255, 255];

            try {
              dominante = colorThief.getColor(imgElement);
            } catch (e) {
              console.warn(
                'Non è stato possibile estrarre il colore dominante',
                e,
              );
            }

            const coloreInvertito = this.invertiColore(dominante);

            const nuovaImmagine: ImmagineForm = {
              alt: '',
              caption: '',
              isCover: false,
              nomeFileSeo: '',
              ordine: this.immagini.length,
              title: '',
              file,
              anteprima,
              url: '',
              coloreWarning: coloreInvertito,
            };

            this.immagini.push(nuovaImmagine);
            this.immaginiAggiornate.emit(this.immagini);
          };
        };
        reader.readAsDataURL(file);
      });

      if ((event.target as HTMLInputElement).value) {
        (event.target as HTMLInputElement).value = '';
      }
    }
  }

  invertiColore(rgb: number[]): string {
    const [r, g, b] = rgb;
    return `rgb(${255 - r}, ${255 - g}, ${255 - b})`;
  }

  // ======= MODALI =======
  apriZoom(img: ImmagineForm) {
    this.immagineZoom = img.anteprima || img.url;
    this.modaleZoomAttivo = true;
  }

  chiudiZoom() {
    this.modaleZoomAttivo = false;
    this.immagineZoom = undefined;
  }

  apriModaleMetadati(img: ImmagineForm, index: number) {
    this.immagineSelezionata = { ...img };
    this.indiceSelezionato = index;
    this.modaleAttivo = true;
  }

  salvaMetadati(modificata: ImmagineForm) {
    if (this.indiceSelezionato !== undefined) {
      const immagineEsistente = this.immagini[this.indiceSelezionato];

      this.immagini[this.indiceSelezionato] = {
        ...immagineEsistente,
        ...modificata,
        alt: modificata.alt,
        caption: modificata.caption,
        isCover: modificata.isCover,
        nomeFileSeo: modificata.nomeFileSeo,
        title: modificata.title,
        ordine: modificata.ordine,
      };

      this.immaginiAggiornate.emit(this.immagini);
    }

    this.modaleAttivo = false;
    this.immagineSelezionata = undefined;
    this.indiceSelezionato = undefined;
  }

  // ======= TOOLTIP SEO =======
  mostraTooltip(index: number): void {
    this.tooltipVisibile = true;
    this.indiceTooltip = index;
  }

  nascondiTooltip(): void {
    this.tooltipVisibile = false;
    this.indiceTooltip = null;
  }

  hasMetadatiMancanti(img: ImmagineForm): boolean {
    return !img.alt?.trim() || !img.title?.trim();
  }

  // ======= GESTIONE DRAG IMMAGINI =======
  debugMode = true;

  startDrag(event?: CdkDragStart) {
    this.inDragInterno = true;
    const grid = document.querySelector('.immagini-grid') as HTMLElement;
    grid.classList.add('drag-attiva');
    grid.style.minHeight = `${grid.offsetHeight}px`;
    document.body.style.overflow = 'hidden';

    // Aggiunge .dragging alla card in movimento
    if (event) {
      const el = event.source.element.nativeElement;
      el.classList.add('dragging');
      console.log('✅ .dragging aggiunta a:', el);
    }
  }

  stopDrag() {
    this.inDragInterno = false;
    this.dropzoneAttiva = false;
    this.indiceInserimento = null;
    const grid = document.querySelector('.immagini-grid') as HTMLElement;
    grid.classList.remove('drag-attiva');
    grid.style.minHeight = '';
    document.body.style.overflow = '';
    clearInterval(this.scrollInterval);
    document
      .querySelectorAll('.drag-over-row')
      .forEach((el) => el.classList.remove('drag-over-row'));

    document
      .querySelectorAll('.card-wrapper.dragging')
      .forEach((el) => el.classList.remove('dragging'));
  }

  onDragMoved(event: CdkDragMove<any>) {
    if (!this.inDragInterno) return;

    const grid = document.querySelector('.immagini-grid') as HTMLElement;
    const cardWrappers = Array.from(
      grid.querySelectorAll('.card-wrapper'),
    ) as HTMLElement[];

    const pointerX = event.pointerPosition.x;
    const pointerY = event.pointerPosition.y;

    const righe: HTMLElement[][] = [];
    let rigaCorrente: HTMLElement[] = [];
    let yPrecedente: number | null = null;

    cardWrappers.forEach((card) => {
      const y = card.getBoundingClientRect().top;
      if (yPrecedente === null || Math.abs(y - yPrecedente) < 10) {
        rigaCorrente.push(card);
      } else {
        righe.push(rigaCorrente);
        rigaCorrente = [card];
      }
      yPrecedente = y;
    });
    if (rigaCorrente.length) righe.push(rigaCorrente);

    let newIndex = 0;
    let globalIndex = 0;
    let found = false;

    for (const riga of righe) {
      const firstRect = riga[0].getBoundingClientRect();
      const lastRect = riga[riga.length - 1].getBoundingClientRect();

      if (pointerY >= firstRect.top && pointerY <= lastRect.bottom) {
        for (let j = 0; j < riga.length; j++) {
          const rect = riga[j].getBoundingClientRect();
          if (pointerX < rect.left + rect.width / 2) {
            newIndex = globalIndex + j;
            found = true;
            break;
          }
        }
        if (!found) {
          newIndex = globalIndex + riga.length;
          found = true;
        }
        break;
      }
      globalIndex += riga.length;
    }

    const lastCard = cardWrappers[cardWrappers.length - 1];
    if (!found && pointerY > lastCard.getBoundingClientRect().bottom) {
      newIndex = cardWrappers.length;
    }

    const gridRect = grid.getBoundingClientRect();
    const wrapperRef =
      cardWrappers[Math.min(newIndex, cardWrappers.length - 1)];
    const rectRef =
      wrapperRef?.getBoundingClientRect() ?? lastCard.getBoundingClientRect();

    this.indiceInserimento = newIndex;
    this.posizioneInserimentoPx = rectRef.left - gridRect.left;
    this.posizioneInserimentoTopPx = rectRef.top - gridRect.top;

    cardWrappers.forEach((w) => w.classList.remove('moving'));
    if (cardWrappers[newIndex - 1]) {
      cardWrappers[newIndex - 1].classList.add('moving');
    }
    if (cardWrappers[newIndex]) {
      cardWrappers[newIndex].classList.add('moving');
    }

    if (this.debugMode) {
      console.log('[onDragMoved] indiceInserimento:', newIndex);
    }

    this.autoScroll(pointerY);
  }

  spostaImmagine(event: CdkDragDrop<ImmagineForm[]>) {
    const daIndice = event.previousIndex;
    const aIndice = this.indiceInserimento;

    if (this.debugMode) {
      console.log('📦 [DROP] from:', daIndice, 'to:', aIndice);
    }

    if (aIndice === null || daIndice === aIndice || daIndice === aIndice - 1) {
      this.stopDrag();
      return;
    }

    const item = this.immagini.splice(daIndice, 1)[0];
    const targetIndex = daIndice < aIndice ? aIndice - 1 : aIndice;
    this.immagini.splice(targetIndex, 0, item);
    this.immagini.forEach((img, i) => (img.ordine = i));
    this.immaginiAggiornate.emit(this.immagini);

    if (this.debugMode) {
      console.log(
        '[ORDINE]',
        this.immagini.map((i) => i.title),
      );
    }

    setTimeout(() => this.stopDrag(), 10);
  }

  autoScroll(pointerY: number) {
    cancelAnimationFrame(this.scrollInterval);
    const margin = 100;
    const speed = 35;

    const scrollLoop = () => {
      const windowHeight = window.innerHeight;
      if (pointerY < margin) {
        window.scrollBy({ top: -speed, behavior: 'auto' });
        this.scrollInterval = requestAnimationFrame(scrollLoop);
      } else if (pointerY > windowHeight - margin) {
        window.scrollBy({ top: speed, behavior: 'auto' });
        this.scrollInterval = requestAnimationFrame(scrollLoop);
      }
    };

    this.scrollInterval = requestAnimationFrame(scrollLoop);
  }

  rimuovi(index: number): void {
    this.immagini.splice(index, 1);
    this.immagini.forEach((img, i) => (img.ordine = i));
    this.immaginiAggiornate.emit(this.immagini);
  }
}
