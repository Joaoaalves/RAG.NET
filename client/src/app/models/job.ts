import { Document } from './document';

export type JobStatus = 'Pending' | 'Processing' | 'Done' | 'Error';

export interface JobProcess {
  title: string;
  progress: number;
}

export interface JobItem {
  jobId: string;
  status: JobStatus;
  process: JobProcess;
  document: Document;
  error?: string;
}

export interface JobNotificationResponse {
  jobId: string;
  userId: string;
  document: Document;
  process: JobProcess;
}

export interface PendingJob {
  id: string;
  workflowId: string;
}

export interface PendingJobsResponse {
  pendingJobs: PendingJob[];
}
