import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { DashboardLayoutComponent } from './components/dashboard/dashboard-layout.component';
import { AuthGuard } from './guards/auth.guard';
import { RegisterComponent } from './components/register/register.component';
import { NewWorkflowComponent } from './components/new-workflow/new-workflow.component';
import { EmbeddingUploadComponent } from './components/embedding/embedding-upload.component';
import { WorkflowsComponent } from './components/workflows/workflows.component';
import { WorkflowComponent } from './components/workflow/workflow.component';
import { QueryComponent } from './components/workflow/query/query.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent, title: 'Login - RAG.NET' },
  {
    path: 'register',
    component: RegisterComponent,
    title: 'Register - RAG.NET',
  },
  {
    path: 'dashboard',
    component: DashboardLayoutComponent,
    title: 'Dashboard - RAG.NET',
    canActivate: [AuthGuard],
    children: [
      {
        path: 'workflows',
        component: WorkflowsComponent,
        canActivate: [AuthGuard],
        title: 'Workflows - RAG.NET',
      },
      {
        path: 'workflows/new',
        component: NewWorkflowComponent,
        title: 'New Workflow - RAG.NET',
        canActivate: [AuthGuard],
      },
      {
        path: 'workflows/:workflowId/embedd',
        component: EmbeddingUploadComponent,
        title: 'Embedding - RAG.NET',
        canActivate: [AuthGuard],
      },
      {
        path: 'workflows/:workflowId',
        canActivate: [AuthGuard],
        title: 'Workflow - RAG.NET',
        component: WorkflowComponent,
      },
      {
        path: 'workflows/:workflowId/query',
        canActivate: [AuthGuard],
        title: 'Query - RAG.NET',
        component: QueryComponent,
      },
    ],
  },
];
