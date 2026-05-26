import axios from 'axios'; 
import type { AxiosInstance, AxiosError } from 'axios';
import { config }  from "../config/env";
import type { ApiErrorResponse } from '../types/api.types';


const apiClient: AxiosInstance = axios.create({
    baseURL: config.apiBaseUrl || "http://localhost:8000/api",
    headers: {
        "Content-Type": "application/json",
    },
    timeout: 10000,
});

apiClient.interceptors.request.use(
    (config) => {
        const token = localStorage.getItem("auth_token");
        if (token) {
            config.headers["Authorization"] = `Bearer ${token}`;
        }   
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

apiClient.interceptors.response.use(
    (response) => response,
    (error: AxiosError<ApiErrorResponse>) => {
        if (error.response?.status === 401) {
            localStorage.removeItem("auth_token");
            localStorage.removeItem("user_data");
            window.location.href = "/login";
        }
        if (error.response?.status && error.response.status >= 500) {
            console.error("Server error:", error.response.data);
            alert("A server error occurred. Please try again later.");
        }
        return Promise.reject(error);
    }
);

export default apiClient;