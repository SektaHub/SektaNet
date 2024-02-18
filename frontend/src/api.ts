export const fetchWithAuth = async (url: string, options: RequestInit = {}): Promise<Response> => {
  const token: string | null = localStorage.getItem('accessToken');
  const headers: HeadersInit = {
    ...options.headers,
    Authorization: `Bearer ${token}`,
  };

  const response: Response = await fetch(`${url}`, {
    ...options,
    headers,
  });

  // If response status is 401, redirect to login page
  if (response.status === 401) {
    window.location.href = '/login';
  }

  return response;
};
