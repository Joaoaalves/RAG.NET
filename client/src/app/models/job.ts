import { Document } from './document';

export type JobStatus = 'Pending' | 'Processing' | 'Done' | 'Error';

export interface JobItem {
  jobId: string;
  status: JobStatus;
  document: Document;
  error?: string;
}

export interface JobNotificationResponse {
  jobId: string;
  userId: string;
  document: Document;
}
