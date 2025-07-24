import { Component } from '@angular/core';
import { ToasterService } from '../../shared/services/toaster.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-toaster-example',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="container mt-4">
      <h2>Toaster Service Example</h2>

      <div class="row mt-4">
        <div class="col-md-6">
          <div class="card">
            <div class="card-header">
              Test Toaster Messages
            </div>
            <div class="card-body">
              <button class="btn btn-success me-2 mb-2" (click)="showSuccess()">Success Message</button>
              <button class="btn btn-danger me-2 mb-2" (click)="showError()">Error Message</button>
              <button class="btn btn-info me-2 mb-2" (click)="showInfo()">Info Message</button>
              <button class="btn btn-warning me-2 mb-2" (click)="showWarning()">Warning Message</button>
              <button class="btn btn-secondary me-2 mb-2" (click)="simulateHttpError()">Simulate HTTP Error</button>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .card {
      box-shadow: 0 4px 8px rgba(0,0,0,0.1);
    }
  `]
})
export class ToasterExampleComponent {

  constructor(private toasterService: ToasterService) {}

  showSuccess() {
    this.toasterService.success('Operation completed successfully!', 'Success');
  }

  showError() {
    this.toasterService.error('Something went wrong!', 'Error');
  }

  showInfo() {
    this.toasterService.info('Here is some important information', 'Info');
  }

  showWarning() {
    this.toasterService.warning('This action might have consequences', 'Warning');
  }

  simulateHttpError() {
    // Simulate an HTTP error
    const mockError = {
      error: {
        message: 'Server returned a 404 Not Found error'
      },
      status: 404,
      statusText: 'Not Found'
    };

    this.toasterService.handleError(mockError);
  }
}
