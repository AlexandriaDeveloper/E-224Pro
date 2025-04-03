import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot, Router, RouterStateSnapshot } from '@angular/router';

@Component({
  selector: 'app-form-details',
  standalone: false,
  templateUrl: './form-details.component.html',
  styleUrl: './form-details.component.scss'
})
export class FormDetailsComponent implements OnInit {
  router = inject(ActivatedRoute);
  id;
  ngOnInit(): void {
    this.router.params.subscribe(params => this.id = params['id']);

  }

}
