import { useEffect } from 'react';

export default function useInfiniteScroll(ref, callback, deps = []) {
  useEffect(() => {
    const el = ref.current;
    if (!el) return;
    const handleScroll = () => {
      const { scrollTop, scrollHeight, clientHeight } = el;
      if (scrollHeight - scrollTop - clientHeight < 200) {
        callback();
      }
    };
    el.addEventListener('scroll', handleScroll);
    return () => el.removeEventListener('scroll', handleScroll);
    // eslint-disable-next-line
  }, deps);
}
