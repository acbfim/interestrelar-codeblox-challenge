const protocol = 'http';
const server = 'localhost';
const port = '1123';
const host = `${protocol}://${server}:${port}`;

export const environment = {
  production: false,
  ambiente: 'Development',
  ambienteAbrev: 'DEV',
  urlApi: `${host}/api`,
  secretKey: 'aA8&6ahSh$#o@a(23)K8t$#',
};
