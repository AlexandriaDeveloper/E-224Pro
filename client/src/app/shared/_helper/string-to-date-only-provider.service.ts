import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class StringToDateOnlyProviderService {

  constructor() { }
  public stringToDateOnlyProvider(dateString: string): string {
    const date = new Date(dateString);
    const year = date.getFullYear();
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const day = date.getDate().toString().padStart(2, '0');
    return `${year}-${month}-${day}`;
  }
}
