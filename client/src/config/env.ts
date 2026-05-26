export const config = {
  apiBaseUrl: import.meta.env.VITE_API_BASE_URL,
  appName: import.meta.env.VITE_APP_NAME,
  backendUrl: import.meta.env.VITE_BACKEND_URL,

  validateEnv() {
    if (!this.apiBaseUrl) {
      throw new Error("VITE_API_BASE_URL is not defined in the environment variables.");
    }
  }
};

config.validateEnv();