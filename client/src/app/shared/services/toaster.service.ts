import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class ToasterService {

  constructor(private toastr: ToastrService) { }

  /**
   * Show success message
   * @param message Message to display
   * @param title Optional title
   */
  success(message: string, title?: string): void {
    this.toastr.success(message, title);
  }

  /**
   * Show error message
   * @param message Message to display
   * @param title Optional title
   */
  error(message: string, title: string = 'Error'): void {
    this.toastr.error(message, title);
  }

  /**
   * Show info message
   * @param message Message to display
   * @param title Optional title
   */
  info(message: string, title?: string): void {
    this.toastr.info(message, title);
  }

  /**
   * Show warning message
   * @param message Message to display
   * @param title Optional title
   */
  warning(message: string, title?: string): void {
    this.toastr.warning(message, title);
  }

  /**
   * Handle error message from HTTP or other errors
   * @param error Error object to parse and display
   */
  handleError(error: any): void {
    let errorMessage = 'An unknown error occurred';

    if (error) {
      // Handle HTTP errors
      if (error.error?.message) {
        errorMessage = error.error.message;
      } else if (error.message) {
        errorMessage = error.message;
      } else if (typeof error === 'string') {
        errorMessage = error;
      }
    }

    this.error(errorMessage);
  }
}
