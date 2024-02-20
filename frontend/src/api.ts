export interface CustomRequestInit extends RequestInit {
  disableRedirect?: boolean;
}

export const fetchWithAuth = async (url: string, options: CustomRequestInit = {}): Promise<Response> => {
  const token: string | null = localStorage.getItem('accessToken');
  const headers: HeadersInit = {
    ...options.headers,
    Authorization: `Bearer ${token}`,
  };

  const response: Response = await fetch(`${url}`, {
    ...options,
    headers,
  });

  // If response status is 401 and the request was not initiated from the logout function, redirect to login page
  if (response.status === 401 && !options.disableRedirect) {
    // Redirect to login page
    window.location.href = '/login';
  }

  return response;
};




export const handleLogout = () => {
  localStorage.removeItem('accessToken'); // Clear access token
  window.location.href = '/login'; // Redirect to login page
};
