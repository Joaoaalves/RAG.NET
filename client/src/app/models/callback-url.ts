export interface CallbackUrl {
  id: string;
  url: string;
}
export interface AddCallbackUrlResponse {
  message?: string;
  url: CallbackUrl;
}
