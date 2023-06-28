import React, { useEffect } from 'react'
import { Grid, GridColumn } from 'semantic-ui-react'
import { useStore } from '../../../app/stores/store';
import LoadingComponent from '../../../app/layout/LoadingComponent';
import { observer } from 'mobx-react-lite';
import { useParams } from 'react-router-dom';
import ActivityDetailedInfo from './ActivityDetailedInfo';
import  ActivityDetailedChat from './ActivityDetailedChat';
import ActivityDetailedSidebar from './ActivityDetailedSidebar';
import ActivityDetailedHeader from './ActivityDetailedHeader';



export default observer(function ActivityDetails() {

    
    const {activityStore} = useStore();
    const {selectedActivity, loadActivity, loadingInitial} = activityStore;
    const {id} = useParams();

    useEffect(() => {
        if(id) loadActivity(id);
    }, [id, loadActivity])
    

    if(loadingInitial || !selectedActivity) return <LoadingComponent />;
    
    return (
        <Grid>
            <GridColumn width={10}>
                <ActivityDetailedHeader activity={selectedActivity} />
                <ActivityDetailedInfo activity={selectedActivity} />
                <ActivityDetailedChat />
            </GridColumn>
            <GridColumn width={6}>
                <ActivityDetailedSidebar />
            </GridColumn>
        </Grid>
    )
})
