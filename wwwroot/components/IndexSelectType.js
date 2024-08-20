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

const indexSelectTypeOption = defineAsyncComponent(async () => {
    const template = await loadTemplate('/templates/indexSelectType.html');
    return {
        template,
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

const indexSelectOne = (fetchData) => {
    const selectType = reactive({});

    const fnChangeText = (text) => {
        selectType.defaultText = text;
    };

    const onMounted = async () => {
        try {
            const res = await fetchData();
            if (res.state) {
                Object.assign(selectType, res.data);
            }
        } catch (err) {
            throw new Error(`HTTP error! status: ${err}`);
        }
    }

    return {
        selectType,
        fnChangeText,
        onMounted
    };
}

const indexSelectMore = (ArrayFunction) => {
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
                throw new Error(`HTTP error! status: ${err}`);
            }
        }
    };

    return {
        selectTypes,
        fnChangeText,
        onMounted
    }
}