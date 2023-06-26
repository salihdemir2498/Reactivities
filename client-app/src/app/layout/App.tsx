import { Container } from 'semantic-ui-react';
import NavBar from './NavBar';
import { observer } from 'mobx-react-lite';
import { Outlet, useLocation } from 'react-router-dom';
import HomePage from '../../features/home/HomePage';

function App() {
  const location = useLocation();

  return (
    <>
      {location.pathname === '/' ? <HomePage /> : (
        <>
          <NavBar />
          <Container style={{ marginTop: '7em' }}>
            <Outlet />
          </Container>
        </>
      )}


    </>
  );
}

export default observer(App);
//observer fonksiyonu, MobX ile entegre olarak çalışan React bileşenlerini sarmalamak için kullanılan bir yüksek düzey bileşendir. observer,
//MobX tarafından izlenen bir bileşenin otomatik olarak yeniden render edilmesini sağlar, böylece bileşenin durumu değiştiğinde güncellenmesi 
//gerekmez.
