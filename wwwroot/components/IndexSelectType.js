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

    const setDefaultConfig = (config) => {
        let data = {};
        if (config === "sport") {
            data = {
                iconClass: 'fa-solid fa-medal',   //圖標class
                defaultText: '選擇運動', //預設文字
                listItem: []
            };
        }
        else if (config === "area") {
            data = {
                iconClass: 'fa-solid fa-map-pin',   //圖標class
                defaultText: '選擇地區', //預設文字
                listItem: []
            };
        }
        return data;
    }

    for (const { fetchData, config } of ArrayFunction) {
        selectTypes.push({ data: setDefaultConfig(config), fetchData });
    }

    const fnChangeText = (index, text) => {
        selectTypes[index].data.defaultText = text;
    };

    const onBeforeMount = async () => {
        for (const type of selectTypes) {
            try {
                const res = await type.fetchData();
                if (res) {
                    Object.assign(type.data.listItem, res.data);
                }
            } catch (error) {
                console.log(error);
            }
        }
    };

    return {
        selectTypes,
        fnChangeText,
        onBeforeMount
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