FROM node:22 AS build
WORKDIR /app

RUN npm install -g pnpm

COPY package*.json ./
RUN pnpm install

COPY . .
RUN pnpm run build

FROM nginx:latest AS ngi

COPY --from=build app/dist/client/browser /usr/share/nginx/html
COPY /nginx.conf  /etc/nginx/conf.d/default.conf

EXPOSE 80
