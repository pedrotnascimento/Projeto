export const getHeaders = () => {
  const token: string = localStorage.getItem("accessToken") ?? "";
  const headers = { headers: { "authorization": `Bearer ${token}` } };
  return headers;
}
