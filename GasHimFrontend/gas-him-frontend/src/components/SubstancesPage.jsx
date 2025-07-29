
import React, { useEffect, useState, useRef, useCallback } from 'react';
import { getSubstancesPaged } from '../services/api';
import './SubstancesPage.css';

function SubstancesPage() {
  const [substances, setSubstances] = useState([]);
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
      const data = await getSubstancesPaged({ search, take: 50 });
      setSubstances(data.items || []);
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
      const data = await getSubstancesPaged({ search, take: 50, cursor });
      setSubstances(prev => [...prev, ...(data.items || [])]);
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
    <div className="substances-page">
      <h2>Вещества</h2>
      <div style={{ display: 'flex', gap: 8, marginBottom: 8 }}>
        <input
          type="text"
          placeholder="Поиск..."
          value={search}
          onChange={e => setSearch(e.target.value)}
          className="substances-search"
        />
        <button onClick={fetchFirstPage} style={{ padding: '8px 16px' }}>Обновить</button>
      </div>
      {error && <div className="error">{error}</div>}
      <div ref={listRef} style={{ maxHeight: 500, overflowY: 'auto', border: '1px solid #eee', borderRadius: 4 }}>
        <ul className="substances-list">
          {substances.map(sub => (
            <li key={sub.id} className="substance-item">
              <b>{sub.name}</b>
            </li>
          ))}
        </ul>
        {loading && <div style={{ padding: 10 }}>Загрузка...</div>}
        {!hasMore && !loading && substances.length > 0 && <div style={{ padding: 10, color: '#888' }}>Все данные загружены</div>}
      </div>
    </div>
  );
}

export default SubstancesPage;
