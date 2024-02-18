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
    // Redirect to login page
    setTimeout(() => {
      window.location.href = '/login';
    }, 0);
    
    // Show a popup indicating the user needs to be logged in
    alert('You need to be logged in to do that.');
  }

  return response;
};
