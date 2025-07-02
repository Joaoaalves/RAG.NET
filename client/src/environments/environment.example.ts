export const environment = {
  production: false,
  get apiUrl() {
    return `${window.location.protocol}//${window.location.hostname}:5000`;
  },
};
