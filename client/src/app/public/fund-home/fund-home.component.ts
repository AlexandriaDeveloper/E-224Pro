import { Component, inject, OnInit } from '@angular/core';
import { FundService } from '../../shared/services/fund.service';
import { Fund } from '../../shared/_models/fund.model';
import { CollageService } from '../../shared/services/collage.service';
import { Collage } from '../../shared/_models/collage.model';
import { MatDialog } from '@angular/material/dialog';
import { AddFundDialogComponent } from './add-fund-dialog/add-fund-dialog.component';

@Component({
  selector: 'app-fund-home',
  standalone: false,
  templateUrl: './fund-home.component.html',
  styleUrl: './fund-home.component.scss'
})
export class FundHomeComponent implements OnInit {
  fundService = inject(FundService);
  collageService = inject(CollageService); // Assuming you have a service for collages
  readonly dialog = inject(MatDialog);
  displayedColumns: string[] = ['action', 'fundCode', 'fundName', 'collageName',];
  dataSource: Fund[] = []; // Adjust type as necessary for your fund model
  funds: Fund[] = [];
  collages: Collage[] = []; // Adjust type as necessary for your collage model

  ngOnInit(): void {
    this.loadCollages(); // Load collages when the component initializes
    this.loadFunds();

    // Initialization logic can go here
  }
  loadFunds() {
    this.fundService.getFundsByCollageId({}).subscribe({
      next: (response: Fund[]) => {
        this.funds = response;
        this.dataSource = response; // Assuming the response is an array of Fund objects
      },
      error: (error) => {
        console.error('Error loading funds', error);
      }
    });
  }
  loadCollages() {

    this.collageService.getCollages().subscribe({
      next: (response: Collage[]) => {
        this.collages = response; // Assuming the response is an array of Collage objects
        // Handle the response, e.g., store it in a variable
        console.log('Collages loaded:', response);
      },
      error: (error) => {
        console.error('Error loading collages', error);
      }
    });
  }
  getCollageNameById(collageId: number): string {
    const collage = this.collages.find(c => c.id === collageId);
    return collage ? collage.collageName : 'Unknown Collage'; // Return a default value if not found
  }
  openAddDialog(element: Fund = null) {
    const dialogRef = this.dialog.open(AddFundDialogComponent, {
      data: {
        element: element,
        collages: this.collages
      },

      disableClose: true,
      hasBackdrop: true
    });

    dialogRef.afterClosed().subscribe(result => {
      this.loadFunds();
    });
  }

}
