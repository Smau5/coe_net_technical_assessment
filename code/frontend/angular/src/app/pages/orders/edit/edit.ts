import { HttpClient } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { ActivatedRoute, Router } from '@angular/router';

interface Status {
  value: string;
  name: string;
}

interface Order {
  id: string;
  status: string;
  customer: Customer;
}

interface Customer {
  id: number;
  name: string;
}

@Component({
  selector: 'app-edit',
  imports: [
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    ReactiveFormsModule,
  ],
  templateUrl: './edit.html',
  styleUrl: './edit.css',
})
export class Edit {
  private http = inject(HttpClient);
  private router = inject(Router);
  readonly userId: string | null = '';
  private route = inject(ActivatedRoute);
  protected customerName = signal('');
  status: Status[] = [
    { value: 'Pending', name: 'Pending' },
    { value: 'Completed', name: 'Completed' },
    { value: 'Cancelled', name: 'Cancelled' },
  ];

  orderForm = new FormGroup({
    status: new FormControl(''),
  });

  constructor() {
    this.userId = this.route.snapshot.paramMap.get('id');
  }

  ngOnInit(): void {
    if (this.userId == null) {
      return;
    }
    this.http
      .get<Order>(`https://localhost:7189/api/orders/${this.userId}`)
      .subscribe({
        next: (response) => {
          if (response?.status) {
            this.orderForm.patchValue({
              status: response.status,
            });
          }

          if (response?.customer?.name) {
            this.customerName.set(response?.customer?.name);
          }
        },
        error: (error) => {
          console.log(error);
        },
      });
  }
  onSubmit() {
    this.http
      .put(`https://localhost:7189/api/orders/${this.userId}`, {
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
