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

  return response;
};