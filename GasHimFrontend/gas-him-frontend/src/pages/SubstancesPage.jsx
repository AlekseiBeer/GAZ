import React, { useEffect, useState, useRef, useCallback } from 'react';
import { getSubstancesPaged } from '../services/api';
import Card from '../components/ui/Card';
import Modal from '../components/ui/Modal';
import useInfiniteScroll from '../hooks/useInfiniteScroll';
import '../styles/ListPage.css';

const SubstancesPage = () => {
  const [substances, setSubstances] = useState([]);
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

  useInfiniteScroll(listRef, loadMore, [cursor, hasMore, loading, search]);

  return (
    <div className="list-page">
      <h2>Вещества</h2>
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
        {substances.map(sub => (
          <Card key={sub.id} onClick={() => setSelected(sub)}>
            <b>{sub.name}</b>
            {sub.synonyms && <div className="card-secondary">Синонимы: {sub.synonyms}</div>}
          </Card>
        ))}
        {loading && <div style={{ padding: 10 }}>Загрузка...</div>}
        {!hasMore && !loading && substances.length > 0 && <div className="list-page-end">Все данные загружены</div>}
      </div>
      <Modal open={!!selected} onClose={() => setSelected(null)}>
        {selected && (
          <div>
            <h3>{selected.name}</h3>
            {selected.synonyms && <div>Синонимы: {selected.synonyms}</div>}
            <div>ID: {selected.id}</div>
          </div>
        )}
      </Modal>
    </div>
  );
};

export default SubstancesPage;
