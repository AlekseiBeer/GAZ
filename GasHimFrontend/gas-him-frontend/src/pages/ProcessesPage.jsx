import React, { useEffect, useState, useRef, useCallback } from 'react';
import { getProcessesPaged } from '../services/api';
import Card from '../components/ui/Card';
import Modal from '../components/ui/Modal';
import useInfiniteScroll from '../hooks/useInfiniteScroll';
import '../styles/ListPage.css';

const ProcessesPage = () => {
  const [processes, setProcesses] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [search, setSearch] = useState('');
  const [cursor, setCursor] = useState(null);
  const [hasMore, setHasMore] = useState(true);
  const [selected, setSelected] = useState(null);
  const listRef = useRef(null);

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

  useInfiniteScroll(listRef, loadMore, [cursor, hasMore, loading, search]);

  return (
    <div className="list-page">
      <h2>Процессы</h2>
      <div className="list-page-controls">
        <input
          type="text"
          placeholder="Поиск..."
          value={search}
          onChange={e => setSearch(e.target.value)}
          className="list-page-search"
        />
        <button onClick={fetchFirstPage} className="list-page-refresh">Обновить</button>
      </div>
      {error && <div className="error">{error}</div>}
      <div ref={listRef} className="list-page-scroll">
        {processes.map(proc => (
          <Card key={proc.id} onClick={() => setSelected(proc)}>
            <b>{proc.name}</b>
            <div className="card-secondary">Выходы: {proc.primaryProducts || '-'}{proc.byProducts ? `, побочные: ${proc.byProducts}` : ''}</div>
            <div className="card-secondary">Входы: {proc.primaryFeedstocks || '-'}{proc.secondaryFeedstocks ? `, доп: ${proc.secondaryFeedstocks}` : ''}</div>
          </Card>
        ))}
        {loading && <div style={{ padding: 10 }}>Загрузка...</div>}
        {!hasMore && !loading && processes.length > 0 && <div className="list-page-end">Все данные загружены</div>}
      </div>
      <Modal open={!!selected} onClose={() => setSelected(null)}>
        {selected && (
          <div>
            <h3>{selected.name}</h3>
            <div><b>Основные входы:</b> {selected.primaryFeedstocks || '-'}</div>
            <div><b>Доп. входы:</b> {selected.secondaryFeedstocks || '-'}</div>
            <div><b>Основные продукты:</b> {selected.primaryProducts || '-'}</div>
            <div><b>Побочные продукты:</b> {selected.byProducts || '-'}</div>
            <div><b>Выход, %:</b> {selected.yieldPercentage}</div>
            <div>ID: {selected.id}</div>
          </div>
        )}
      </Modal>
    </div>
  );
};

export default ProcessesPage;
