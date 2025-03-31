import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { AuthGuard } from './guards/auth.guard';
import { RegisterComponent } from './components/register/register.component';
import { NewWorkflowComponent } from './components/new-workflow/new-workflow.component';
import { EmbeddingUploadComponent } from './components/embedding/embedding-upload.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent, title: 'Login - RAG.NET' },
  {
    path: 'register',
    component: RegisterComponent,
    title: 'Register - RAG.NET',
  },
  {
    path: 'dashboard',
    component: DashboardComponent,
    title: 'Dashboard - RAG.NET',
    canActivate: [AuthGuard],
  },
  {
    path: 'dashboard/workflows/new',
    component: NewWorkflowComponent,
    title: 'New Workflow - RAG.NET',
    canActivate: [AuthGuard],
  },
  {
    path: 'dashboard/workflows/:workflowId/embedd',
    component: EmbeddingUploadComponent,
    title: 'Embedding - RAG.NET',
    canActivate: [AuthGuard],
  },
];
