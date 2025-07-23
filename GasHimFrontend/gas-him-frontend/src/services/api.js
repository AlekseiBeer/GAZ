const API_BASE_URL = 'http://localhost:5275/api';

export async function getSubstances() {
  const response = await fetch(`${API_BASE_URL}/substances`);
  if (!response.ok) {
    throw new Error("Не удалось получить список веществ");
  }
  return response.json();
}

export async function synthesizeChains(startSubstance, targetSubstance) {
  const payload = { startSubstance, targetSubstance };
  const response = await fetch(`${API_BASE_URL}/chains`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(payload)
  });
  if (!response.ok) {
    throw new Error("Ошибка синтеза цепочек");
  }
  return response.json();
}

export async function synthesizeAllChains() {
  const response = await fetch(`${API_BASE_URL}/chains/all`);
  if (!response.ok) {
    throw new Error("Ошибка синтеза всех цепочек");
  }
  return response.json();
}

export async function getProcessByName(name) {
    const response = await fetch(`${API_BASE_URL}/processes/byname/${name}`);
    if (!response.ok) {
      throw new Error("Не удалось найти процесс по имени");
    }
    return response.json();
  }