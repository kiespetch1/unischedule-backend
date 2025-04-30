#!/usr/bin/env bash
set -e

services=()

while [[ "$1" =~ ^[^-]+:[0-9]+$ ]]; do
  services+=("$1")
  shift
done

if [[ "$1" == "--" ]]; then
  shift
fi

for service in "${services[@]}"; do
  host="${service%%:*}"
  port="${service##*:}"
  echo "Ждём, пока $host:$port станет доступен..."
  until nc -z "$host" "$port"; do
    sleep 2
  done
  echo "$host:$port готов."
done

exec "$@"
