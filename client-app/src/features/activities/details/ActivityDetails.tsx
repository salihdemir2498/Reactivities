import React, { useEffect } from 'react'
import { Button, Card, Image } from 'semantic-ui-react'
import { useStore } from '../../../app/stores/store';
import LoadingComponent from '../../../app/layout/LoadingComponent';
import { observer } from 'mobx-react-lite';
import { Link, useParams } from 'react-router-dom';



export default observer(function ActivityDetails() {

    
    const {activityStore} = useStore();
    const {selectedActivity, loadActivity, setLoadingInitial, loadingInitial} = activityStore;
    const {id} = useParams();

    useEffect(() => {
        if(id) loadActivity(id);
    }, [id, loadActivity])
    

    if(loadingInitial || !selectedActivity) return <LoadingComponent />;
    
    return (
        <Card fluid>
            <Image src={`/assets/categoryImages/${selectedActivity.category}.jpg`} />
            <Card.Content>
                <Card.Header>{selectedActivity.title}</Card.Header>
                <Card.Meta>
                    <span>{selectedActivity.date}</span>
                </Card.Meta>
                <Card.Description>
                    {selectedActivity.description}
                </Card.Description>
            </Card.Content>
            <Card.Content extra>
                <Button.Group widths='2'>
                    <Button as={Link} to={`/manage/${selectedActivity.id}`} basic color='blue' content='Edit' />
                    <Button as={Link} to={'/activities'} basic color='grey' content='Cancel'  />   
                </Button.Group>
            </Card.Content>
        </Card>
    )
})
