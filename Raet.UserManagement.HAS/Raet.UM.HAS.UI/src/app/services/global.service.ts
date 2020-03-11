import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GlobalService {

  private header = new Subject<string>();
  private header$ = this.header.asObservable();

  private backUrl = new Subject<string>();

  constructor() { }

  setHeader(header: string): void {
    this.header.next(header);
  }

  getHeader(): Observable<string> {
    return this.header.asObservable();
  }

  setbackButton(backUrl: string): void {
    this.backUrl.next(backUrl);
  }

  prevoiusState(): Observable<string> {
    return this.backUrl.asObservable();
  }

}
