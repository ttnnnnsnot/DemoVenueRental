const search = (selectTypes) => {
    const results = selectTypes.map(type => {

        if (isEmptyObject(type.data))
            return '';

        const selectedItem = type.data.listItem.find(item => item.name === type.data.defaultText);
        return selectedItem ? selectedItem : '';
    });

    console.log(`Results: ${results.map(item => item ? item.name : '').join(', ')}`);
}

export default {
    search
}