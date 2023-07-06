import { Header } from 'semantic-ui-react';
import { useStore } from '../../../app/stores/store';
import { observer } from 'mobx-react-lite';
import { ActivityListItem } from './ActivityListItem';
import { Fragment } from 'react';



export default observer(function ActivityList() {

    const { activityStore } = useStore();
    const { groupedActivities, groupActivitiesByDate } = activityStore;

    console.log(groupedActivities);
    // console.log(groupedActivitiesTwo);
    // console.log(groupActivitiesByDate);
    //ikiside çalışır




    return (
        <>
            {groupedActivities.map(([date, activities]) => (
                <Fragment key={date}>
                    <Header sub color='teal'>
                        {date}
                    </Header>

                    {activities.map((activity) => (
                        <ActivityListItem key={activity.id} activity={activity} />
                    ))}

                </Fragment>
            ))}


        </>

    )
})
