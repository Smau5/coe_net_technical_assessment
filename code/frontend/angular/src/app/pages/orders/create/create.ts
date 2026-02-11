import { Component, inject, OnInit, signal } from '@angular/core';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { HttpClient } from '@angular/common/http';
import { MatButtonModule } from '@angular/material/button';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import {Router} from '@angular/router';

interface Status {
  value: string;
  name: string;
}

interface Customer {
  id: number;
  name: string;
}

@Component({
  selector: 'app-create',
  imports: [
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    ReactiveFormsModule,
  ],
  templateUrl: './create.html',
  styleUrl: './create.css',
})
export class Create implements OnInit {
  private http = inject(HttpClient);
  private router = inject(Router);
  status: Status[] = [
    { value: 'Pending', name: 'Pending' },
    { value: 'Completed', name: 'Completed' },
    { value: 'Cancelled', name: 'Cancelled' },
  ];
  customers = signal<Customer[]>([]);

  orderForm = new FormGroup({
    status: new FormControl(''),
    customer: new FormControl(''),
  });

  ngOnInit(): void {
    this.http
      .get<Customer[]>('https://localhost:7189/api/customers')
      .subscribe((response) => {
        this.customers.set(response);
      });
  }
  onSubmit() {
    this.http
      .post('https://localhost:7189/api/orders', {
        customerId: this.orderForm.value.customer?.toString(),
        status: this.orderForm.value.status,
      })
      .subscribe({
        next: (response) => {
          this.router.navigate(['/orders']);
        },
        error: (error) => {
          console.log(error);
        },
      });
  }
}
