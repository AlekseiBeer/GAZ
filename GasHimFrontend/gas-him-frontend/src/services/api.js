// Получить процесс по имени
export async function getProcessByName(name) {
  const response = await fetch(`${API_BASE_URL}/processes/search?term=${encodeURIComponent(name)}`);
  if (!response.ok) throw new Error('Не удалось найти процесс по имени');
  const data = await response.json();
  // API возвращает массив, ищем точное совпадение
  return Array.isArray(data) ? data.find(p => p.name === name) : null;
}

const API_BASE_URL = 'http://localhost:5275/api';


// Вещества
export async function getSubstancesPaged({ search = '', take = 50, cursor = null }) {
  const params = new URLSearchParams();
  if (search) params.append('search', search);
  if (take) params.append('take', take);
  if (cursor) params.append('cursor', cursor);
  const response = await fetch(`${API_BASE_URL}/substances/paged?${params}`);
  if (!response.ok) throw new Error('Ошибка загрузки веществ');
  return response.json();
}

export async function getSubstances() {
  const response = await fetch(`${API_BASE_URL}/substances`);
  if (!response.ok) throw new Error('Не удалось получить список веществ');
  return response.json();
}

export async function searchSubstances(term) {
  const response = await fetch(`${API_BASE_URL}/substances/search?term=${encodeURIComponent(term)}`);
  if (!response.ok) throw new Error('Ошибка поиска веществ');
  return response.json();
}


// Синтез цепочек
export async function synthesizeChains(startSubstance, targetSubstance) {
  const payload = { startSubstance, targetSubstance };
  const response = await fetch(`${API_BASE_URL}/chains`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(payload)
  });
  if (!response.ok) throw new Error('Ошибка синтеза цепочек');
  return response.json();
}


export async function synthesizeAllChains() {
  const response = await fetch(`${API_BASE_URL}/chains/all`);
  if (!response.ok) throw new Error('Ошибка синтеза всех цепочек');
  return response.json();
}


// Процессы
export async function getProcessesPaged({ search = '', take = 50, cursor = null }) {
  const params = new URLSearchParams();
  if (search) params.append('search', search);
  if (take) params.append('take', take);
  if (cursor) params.append('cursor', cursor);
  const response = await fetch(`${API_BASE_URL}/processes/paged?${params}`);
  if (!response.ok) throw new Error('Ошибка загрузки процессов');
  return response.json();
}

export async function getProcesses() {
  const response = await fetch(`${API_BASE_URL}/processes`);
  if (!response.ok) throw new Error('Не удалось получить список процессов');
  return response.json();
}

export async function searchProcesses(term) {
  const response = await fetch(`${API_BASE_URL}/processes/search?term=${encodeURIComponent(term)}`);
  if (!response.ok) throw new Error('Ошибка поиска процессов');
  return response.json();
}

// Тестовые ручки
export async function seedSubstances(count = 1000) {
  const response = await fetch(`${API_BASE_URL}/testdata/seed?count=${count}`, { method: 'POST' });
  if (!response.ok) throw new Error('Ошибка сидирования веществ');
  return response.json();
}

export async function seedProcesses(count = 300) {
  const response = await fetch(`${API_BASE_URL}/testdata/processes/seed?count=${count}`, { method: 'POST' });
  if (!response.ok) throw new Error('Ошибка сидирования процессов');
  return response.json();
}

export async function clearAll() {
  const response = await fetch(`${API_BASE_URL}/testdata/all`, { method: 'DELETE' });
  if (!response.ok) throw new Error('Ошибка очистки всех данных');
}

export async function clearProcesses() {
  const response = await fetch(`${API_BASE_URL}/testdata/processes`, { method: 'DELETE' });
  if (!response.ok) throw new Error('Ошибка очистки процессов');
}