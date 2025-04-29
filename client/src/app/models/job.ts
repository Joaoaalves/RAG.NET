export type JobStatus = 'Pending' | 'Processing' | 'Done' | 'Error';

export interface JobItem {
  jobId: string;
  status: JobStatus;
  error?: string;
}

export interface JobNotificationResponse {
  jobId: string;
  userId: string;
}
