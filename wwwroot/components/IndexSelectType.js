/*
props
selectType = {
    iconClass:'',   //圖標class
    defaultText:'', //預設文字
    listItem:[
        {[selectTypeID], [name]}
    ]     //選項
}
*/

export const indexSelectTypeOption = defineAsyncComponent(async () => {
    return {
        template: await API.GetTemplate('/templates/indexSelectType.html'),
        emits: ['data-change'],
        props: ['selectType'],
        setup(props, { emit }) {
            const fnChangeText = (item) => {
                emit('data-change', item.name);
            };
            return {
                fnChangeText
            };
        }
    };
});

export const indexSelectMore = (ArrayFunction) => {
    const selectTypes = reactive([]);

    for (const fetchData of ArrayFunction) {
        selectTypes.push({ data: {}, fetchData });
    }

    const fnChangeText = (index, text) => {
        selectTypes[index].data.defaultText = text;
    };

    const onMounted = async () => {
        for (const type of selectTypes) {
            try {
                const res = await type.fetchData();
                if (res.state) {
                    Object.assign(type.data, res.data);
                }
            } catch (err) {
                console.log(`HTTP error! status: ${err}`);
            }
        }
    };

    return {
        selectTypes,
        fnChangeText,
        onMounted
    }
}

export const search = (selectTypes) => {
    const results = selectTypes.map(type => {

        if (isEmptyObject(type.data))
            return '';

        const selectedItem = type.data.listItem.find(item => item.name === type.data.defaultText);
        return selectedItem ? selectedItem : '';
    });

    console.log(`Results: ${results.map(item => item ? item.name : '').join(', ')}`);
}