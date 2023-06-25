import React from 'react'
import { Button, Card, Image } from 'semantic-ui-react'
import { useStore } from '../../../app/stores/store';
import LoadingComponent from '../../../app/layout/LoadingComponent';
import { observer } from 'mobx-react-lite';



export default observer(function ActivityDetails() {

    
    const {activityStore} = useStore();
    const {selectedActivity, openForm, cancelSelectedActivity} = activityStore;

    if(!selectedActivity) return <LoadingComponent />;
    
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
                    <Button onClick={() => openForm(selectedActivity.id)} basic color='blue' content='Edit' />
        
                    <Button basic color='grey' content='Cancel' onClick={cancelSelectedActivity} />   {/* onClick={() => cancelSelectActivity()} böylede olur */}
                </Button.Group>
            </Card.Content>
        </Card>
    )
})
