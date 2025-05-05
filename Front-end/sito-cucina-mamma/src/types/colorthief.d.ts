declare module 'colorthief' {
  export default class ColorThief {
    getColor(img: HTMLImageElement, quality?: number): [number, number, number];
  }
}
