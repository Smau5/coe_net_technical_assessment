import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { RouterLink } from '@angular/router';

interface Order {
  id: number;
  status: string;
  customer: Customer;
}

interface Customer {
  id: number;
  name: string;
}

@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [MatTableModule, MatButtonModule, RouterLink],
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.css',
})
export class OrdersComponent implements OnInit {
  private http = inject(HttpClient);

  displayedColumns: string[] = ['id', 'status', 'customer', 'edit', 'delete'];
  dataSource = signal<Order[]>([]);

  ngOnInit(): void {
    this.getData();
  }

  getData() {
    this.http
      .get<Order[]>('https://localhost:7189/api/orders')
      .subscribe((response) => {
        this.dataSource.set(response);
      });
  }

  delete(id: number) {
    this.http
      .delete(`https://localhost:7189/api/orders/${id}`, {
        responseType: 'text',
      })
      .subscribe((response) => {
        this.getData();
      });
  }
}
