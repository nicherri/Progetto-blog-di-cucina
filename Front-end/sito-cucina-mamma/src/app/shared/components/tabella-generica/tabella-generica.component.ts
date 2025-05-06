import {
  Component,
  Input,
  Output,
  EventEmitter,
  ElementRef,
  Renderer2,
  AfterViewInit,
  ViewChild,
} from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-tabella-generica',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './tabella-generica.component.html',
  styleUrls: ['./tabella-generica.component.scss'],
})
export class TabellaGenericaComponent implements AfterViewInit {
  @Input() colonne: { campo: string; intestazione: string }[] = [];
  @Input() dati: any[] = [];
  @ViewChild('tableRef') tableRef!: ElementRef;

  currentSortColumn: number | null = null;
  currentSortDirection: 'asc' | 'desc' = 'asc';
  isResizing = false;

  constructor(private renderer: Renderer2) {}

  ngAfterViewInit() {
    const thElements = this.tableRef.nativeElement.querySelectorAll('th');

    thElements.forEach((th: HTMLElement) => {
      const resizer = this.renderer.createElement('div');
      this.renderer.addClass(resizer, 'resizer');
      this.renderer.appendChild(th, resizer);
      this.renderer.setStyle(th, 'position', 'relative');

      let startX = 0;
      let startWidth = 0;

      const mouseDownHandler = (e: MouseEvent) => {
        this.isResizing = true;
        startX = e.pageX;
        startWidth = th.offsetWidth;

        const mouseMoveHandler = (moveEvent: MouseEvent) => {
          const newWidth = startWidth + (moveEvent.pageX - startX);
          this.renderer.setStyle(th, 'width', `${newWidth}px`);
        };

        const mouseUpHandler = () => {
          this.isResizing = false;
          this.renderer.removeClass(document.body, 'resizing');
          document.removeEventListener('mousemove', mouseMoveHandler);
          document.removeEventListener('mouseup', mouseUpHandler);
        };

        document.addEventListener('mousemove', mouseMoveHandler);
        document.addEventListener('mouseup', mouseUpHandler);
      };

      resizer.addEventListener('mousedown', mouseDownHandler);
    });
  }

  sortTableByColumn(index: number) {
    if (this.isResizing) return;

    const direction =
      this.currentSortColumn === index && this.currentSortDirection === 'asc'
        ? 'desc'
        : 'asc';
    this.currentSortDirection = direction;
    this.currentSortColumn = index;

    this.dati.sort((a, b) => {
      const valA = a[this.colonne[index].campo];
      const valB = b[this.colonne[index].campo];

      if (typeof valA === 'string' && typeof valB === 'string') {
        return direction === 'asc'
          ? valA.localeCompare(valB)
          : valB.localeCompare(valA);
      }

      return direction === 'asc' ? valA - valB : b - a;
    });
  }

  exportToCSV() {
    const headers = this.colonne.map((c) => c.intestazione);
    const rows = this.dati.map((row) => this.colonne.map((c) => row[c.campo]));
    const csv = [headers, ...rows].map((e) => e.join(',')).join('\n');

    const blob = new Blob([csv], { type: 'text/csv' });
    const a = document.createElement('a');
    a.href = URL.createObjectURL(blob);
    a.download = 'esportazione.csv';
    a.click();
  }
}
