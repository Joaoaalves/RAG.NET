export const environment = {
  production: true,
  get apiUrl() {
    return `${window.location.protocol}//${window.location.hostname}:5000`;
  },
};
