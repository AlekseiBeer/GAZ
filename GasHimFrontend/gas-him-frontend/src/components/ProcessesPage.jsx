
import React, { useEffect, useState, useRef, useCallback } from 'react';
import { getProcessesPaged } from '../services/api';
import './ProcessesPage.css';

function ProcessesPage() {
  const [processes, setProcesses] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [search, setSearch] = useState('');
  const [cursor, setCursor] = useState(null);
  const [hasMore, setHasMore] = useState(true);
  const listRef = useRef(null);

  // Загрузка первой страницы или при поиске
  const fetchFirstPage = useCallback(async () => {
    setLoading(true);
    setError(null);
    setCursor(null);
    setHasMore(true);
    try {
      const data = await getProcessesPaged({ search, take: 50 });
      setProcesses(data.items || []);
      setCursor(data.nextCursor || null);
      setHasMore(data.hasMore);
    } catch (e) {
      setError('Ошибка загрузки');
    }
    setLoading(false);
  }, [search]);

  useEffect(() => {
    fetchFirstPage();
  }, [fetchFirstPage]);

  // Подгрузка следующей страницы
  const loadMore = useCallback(async () => {
    if (!hasMore || loading || !cursor) return;
    setLoading(true);
    try {
      const data = await getProcessesPaged({ search, take: 50, cursor });
      setProcesses(prev => [...prev, ...(data.items || [])]);
      setCursor(data.nextCursor || null);
      setHasMore(data.hasMore);
    } catch (e) {
      setError('Ошибка загрузки');
    }
    setLoading(false);
  }, [cursor, hasMore, loading, search]);

  // Обработчик скролла
  useEffect(() => {
    const handleScroll = () => {
      const el = listRef.current;
      if (!el || loading || !hasMore) return;
      const { scrollTop, scrollHeight, clientHeight } = el;
      if (scrollHeight - scrollTop - clientHeight < 200) {
        loadMore();
      }
    };
    const el = listRef.current;
    if (el) el.addEventListener('scroll', handleScroll);
    return () => { if (el) el.removeEventListener('scroll', handleScroll); };
  }, [loadMore, hasMore, loading]);

  return (
    <div className="processes-page">
      <h2>Процессы</h2>
      <div style={{ display: 'flex', gap: 8, marginBottom: 8 }}>
        <input
          type="text"
          placeholder="Поиск..."
          value={search}
          onChange={e => setSearch(e.target.value)}
          className="processes-search"
        />
        <button onClick={fetchFirstPage} style={{ padding: '8px 16px' }}>Обновить</button>
      </div>
      {error && <div className="error">{error}</div>}
      <div ref={listRef} style={{ maxHeight: 500, overflowY: 'auto', border: '1px solid #eee', borderRadius: 4 }}>
        <ul className="processes-list">
          {processes.map(proc => (
            <li key={proc.id} className="process-item">
              <div>
                <b>{proc.name}</b>
              </div>
              {proc.mainInputs && (
                <div className="process-detail">
                  <span className="label">Входы:</span> {proc.mainInputs}
                </div>
              )}
              {proc.mainOutputs && (
                <div className="process-detail">
                  <span className="label">Выходы:</span> {proc.mainOutputs}
                </div>
              )}
            </li>
          ))}
        </ul>
        {loading && <div style={{ padding: 10 }}>Загрузка...</div>}
        {!hasMore && !loading && processes.length > 0 && <div style={{ padding: 10, color: '#888' }}>Все данные загружены</div>}
      </div>
    </div>
  );
}

export default ProcessesPage;
