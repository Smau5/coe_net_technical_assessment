import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { PageNotFoundComponent } from './pages/page-not-found/page-not-found.component';
import { AboutComponent } from './pages/about/about.component';
import { OrdersComponent } from './pages/orders/orders.component';
import { ProductsComponent } from './pages/products/products.component';
import { SupportTicketsComponent } from './pages/support-tickets/support-tickets.component';
import { Create } from './pages/orders/create/create';
import { Edit } from './pages/orders/edit/edit';

export const routes: Routes = [
  { path: 'home', component: HomeComponent },
  { path: 'about', component: AboutComponent },
  { path: 'orders', component: OrdersComponent },
  { path: 'orders/create', component: Create },
  { path: 'orders/edit/:id', component: Edit },
  { path: 'products', component: ProductsComponent },
  { path: 'support-tickets', component: SupportTicketsComponent },
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: '**', component: PageNotFoundComponent },
];
