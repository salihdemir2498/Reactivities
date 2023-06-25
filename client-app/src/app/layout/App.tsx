import { useEffect, useState } from 'react';
import { Container} from 'semantic-ui-react';
import { Activity } from '../models/activity';
import NavBar from './NavBar';
import ActivityDashboard from '../../features/activities/dashboard/ActivityDashboard';
import agent from '../api/agent';
import LoadingComponent from './LoadingComponent';
import { useStore } from '../stores/store';
import { observer } from 'mobx-react-lite';

function App() {
  const {activityStore} = useStore();


  useEffect(() => {
    activityStore.loadActivities();
  }, [activityStore])



  
  


  if (activityStore.loadingInitial) {
    return <LoadingComponent content='Loading app' />
  }

  return (
    <>
      <NavBar />
      <Container style={{ marginTop: '7em' }}>
    
        <ActivityDashboard />
      </Container>

    </>
  );
}

export default observer(App);
//observer fonksiyonu, MobX ile entegre olarak çalışan React bileşenlerini sarmalamak için kullanılan bir yüksek düzey bileşendir. observer, 
//MobX tarafından izlenen bir bileşenin otomatik olarak yeniden render edilmesini sağlar, böylece bileşenin durumu değiştiğinde güncellenmesi 
//gerekmez.
