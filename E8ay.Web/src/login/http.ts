import * as fetch from 'isomorphic-fetch';


export const login = (username: string, password: string) => {
  const body = {
    password,
    username
  }
  return fetch('http://localhost:8100/api/auth/login', {
    body: JSON.stringify(body),
    headers: {
      'Content-Type': 'application/json'
    },
    method: "POST",
  }).then((response: Response) => {
    if (response.status === 401) {
      return Promise.reject('Invalid credential!');
    }
    return response.json();
  })
}
